public class DamageGameEventHandler : StatGameEventHandler
{
    private DamageGameEventSO _eventSO;

    public DamageGameEventHandler(DamageGameEventSO eventSO) : base(eventSO)
    {
        _eventSO = eventSO;
    }
}



