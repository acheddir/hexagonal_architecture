namespace Me.Acheddir.Hexagonal.Persistence.Account;

public class AccountPersistenceAdapter : IAccountLoad, IAccountLock, IAccountUpdate
{
    private readonly IAccountRepository _accountRepository;
    private readonly IActivityRepository _activityRepository;
    private readonly IAccountUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AccountPersistenceAdapter(IAccountRepository accountRepository,
        IActivityRepository activityRepository,
        IAccountUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _accountRepository = accountRepository;
        _activityRepository = activityRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Domain.Account.Account> LoadAccountAsync(AccountId accountId, DateTime baselineDate,
        CancellationToken token)
    {
        var account = await _accountRepository.GetAccountById(accountId.Id, token);
        var ownerActivities = await _activityRepository.GetAccountActivitiesSince(accountId.Id, baselineDate, token);
        var withdrawalBalance = await _activityRepository.GetWithdrawalBalanceUntil(accountId.Id, baselineDate, token);
        var depositBalance = await _activityRepository.GetDepositBalanceUntil(accountId.Id, baselineDate, token);

        return _mapper.Map<Domain.Account.Account>((account, ownerActivities, withdrawalBalance, depositBalance));
    }

    public async Task LockAccountAsync(AccountId accountId, CancellationToken token)
    {
        var accountEntity = await _accountRepository.GetAccountById(accountId.Id, token);

        if (accountEntity == null) throw new AccountNotFoundException(accountId.Id);
        accountEntity.Locked = true;

        await _unitOfWork.Commit();
    }

    public async Task ReleaseAccountAsync(AccountId accountId, CancellationToken token)
    {
        var accountEntity = await _accountRepository.GetAccountById(accountId.Id, token);

        if (accountEntity == null) throw new AccountNotFoundException(accountId.Id);
        accountEntity.Locked = false;

        await _unitOfWork.Commit();
    }

    public async Task UpdateActivitiesAsync(Domain.Account.Account account, CancellationToken token)
    {
        foreach (var activity in account.ActivityWindow.GetActivities())
        {
            if (activity.Id != null) continue;

            var activityEntity = _mapper.Map<ActivityEntity>(activity);
            _accountRepository.Save(activityEntity);
        }

        await _unitOfWork.Commit();
    }
}