namespace CodeWars;

using System.Collections.Generic;

public class TCP {
    public static string TraverseStates(string[] events) {
        var stateMachine = CreateStateMachine();

        IEnumerable<TcpEvent> tcpEvents = events.Select(Enum.Parse<TcpEvent>);
        
        foreach (TcpEvent tcpEvent in tcpEvents)
            if (stateMachine.CanPerformTransition(tcpEvent))
                stateMachine.PerformTransition(tcpEvent);
            else
                return "ERROR";

        return stateMachine.CurrentState.ToString();
    }

    private static StateMachine<TcpState, TcpEvent> CreateStateMachine() =>
        new StateMachine<TcpState, TcpEvent>()
            .WithState(TcpState.CLOSED)
            .WithTransition(TcpState.CLOSED, TcpState.LISTEN, TcpEvent.APP_PASSIVE_OPEN)
            .WithTransition(TcpState.CLOSED, TcpState.SYN_SENT, TcpEvent.APP_ACTIVE_OPEN)
            .WithTransition(TcpState.LISTEN, TcpState.SYN_RCVD, TcpEvent.RCV_SYN)
            .WithTransition(TcpState.LISTEN, TcpState.SYN_SENT, TcpEvent.APP_SEND)
            .WithTransition(TcpState.LISTEN, TcpState.CLOSED, TcpEvent.APP_CLOSE)
            .WithTransition(TcpState.SYN_RCVD, TcpState.FIN_WAIT_1, TcpEvent.APP_CLOSE)
            .WithTransition(TcpState.SYN_RCVD, TcpState.ESTABLISHED, TcpEvent.RCV_ACK)
            .WithTransition(TcpState.SYN_SENT, TcpState.SYN_RCVD, TcpEvent.RCV_SYN)
            .WithTransition(TcpState.SYN_SENT, TcpState.ESTABLISHED, TcpEvent.RCV_SYN_ACK)
            .WithTransition(TcpState.SYN_SENT, TcpState.CLOSED, TcpEvent.APP_CLOSE)
            .WithTransition(TcpState.ESTABLISHED, TcpState.FIN_WAIT_1, TcpEvent.APP_CLOSE)
            .WithTransition(TcpState.ESTABLISHED, TcpState.CLOSE_WAIT, TcpEvent.RCV_FIN)
            .WithTransition(TcpState.FIN_WAIT_1, TcpState.CLOSING, TcpEvent.RCV_FIN)
            .WithTransition(TcpState.FIN_WAIT_1, TcpState.TIME_WAIT, TcpEvent.RCV_FIN_ACK)
            .WithTransition(TcpState.FIN_WAIT_1, TcpState.FIN_WAIT_2, TcpEvent.RCV_ACK)
            .WithTransition(TcpState.CLOSING, TcpState.TIME_WAIT, TcpEvent.RCV_ACK)
            .WithTransition(TcpState.FIN_WAIT_2, TcpState.TIME_WAIT, TcpEvent.RCV_FIN)
            .WithTransition(TcpState.TIME_WAIT, TcpState.CLOSED, TcpEvent.APP_TIMEOUT)
            .WithTransition(TcpState.CLOSE_WAIT, TcpState.LAST_ACK, TcpEvent.APP_CLOSE)
            .WithTransition(TcpState.LAST_ACK, TcpState.CLOSED, TcpEvent.RCV_ACK);
}

public class StateMachine<TState, TTrigger>
    where TState : Enum
    where TTrigger : Enum {
    private TState _currentState;
    private Dictionary<TTrigger, Dictionary<TState, TState>> _transitions = new();
    
    public TState CurrentState => _currentState;

    public StateMachine<TState, TTrigger> WithState(TState newState) {
        _currentState = newState;
        return this;
    }

    public StateMachine<TState, TTrigger> WithTransition(TState from, TState to, TTrigger trigger) {
        if (!_transitions.ContainsKey(trigger))
            _transitions[trigger] = new();
        _transitions[trigger][from] = to;

        return this;
    }

    public bool CanPerformTransition(TTrigger trigger) =>
        _transitions.ContainsKey(trigger) && _transitions[trigger].ContainsKey(_currentState);

    public void PerformTransition(TTrigger trigger) {
        _currentState = _transitions[trigger][_currentState];
    }
}

public enum TcpEvent {
    APP_PASSIVE_OPEN,
    APP_ACTIVE_OPEN,
    APP_SEND,
    APP_CLOSE,
    APP_TIMEOUT,
    RCV_SYN,
    RCV_ACK,
    RCV_SYN_ACK,
    RCV_FIN,
    RCV_FIN_ACK
}

public enum TcpState {
    CLOSED,
    LISTEN,
    SYN_SENT,
    SYN_RCVD,
    ESTABLISHED,
    CLOSE_WAIT,
    LAST_ACK,
    FIN_WAIT_1,
    FIN_WAIT_2,
    CLOSING,
    TIME_WAIT
}