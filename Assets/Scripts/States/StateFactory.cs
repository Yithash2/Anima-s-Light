

using System.Collections.Generic;

public class StateFactory
{
    PlayerStateMachine _context;
    Dictionary<string, BaseState> _states = new Dictionary<string, BaseState>();

    public StateFactory(PlayerStateMachine currentContext){
        _context = currentContext;
        #region Ground States
        _states["jump start"] = new JumpStartState(_context, this);
        _states["grounded"] = new GroundState(_context, this);
        _states["iddle"] = new IddleState(_context, this);
        _states["run"] = new RunState(_context, this);
        _states["dash"] = new DashState(_context, this);
        #endregion
        


        #region Walled States
            _states["walled"] = new WalledState(_context, this);
            _states["wall jump"] = new WallJumpState(_context, this);
            _states["rapel"] = new RapelState(_context, this);
        #endregion

        #region Wet States
            _states["wet"] = new WetState(_context, this);
            _states["swim"] = new SwimState(_context, this);
            _states["sink"] = new SinkState(_context, this);
        #endregion

        #region Air States
            _states["jump end"] = new JumpEndState(_context, this);
            _states["air"] = new AirState(_context, this);
            _states["fall"] = new FallState(_context, this);
            _states["dive"] = new GroundSlamState(_context, this);
            _states["control"] = new ControlState(_context, this);
        #endregion
        
    }

    #region Ground States
        public BaseState Grounded(){
            return _states["grounded"];
        }

        public BaseState Running(){
            return _states["run"];
        }

        public BaseState Iddle(){
            return _states["iddle"]; 
        }

        public BaseState Dash(){
            return _states["dash"];
        }

        public BaseState JumpStart(){
            return _states["jump start"];
        }
    #endregion




        



    #region Walled States
        public BaseState Walled(){
            return _states["walled"];
        }

        public BaseState WallJump(){
            return _states["wall jump"];
        }

        public BaseState Rapelling(){
            return _states["rapel"];
        }
    #endregion

    #region Wet States
        public BaseState Wet(){
            return _states["wet"];
        }

        public BaseState Swimming(){
            return _states["swim"];
        }

        public BaseState Sinking(){
            return _states["sink"];
        }
    #endregion

    #region Air States
        public BaseState Fall(){
            return _states["fall"];
        }

        public BaseState JumpEnd(){
            return _states["jump end"];
        }

        public BaseState Normal(){
            return _states["air"];
        }

        public BaseState Dive(){
            return _states["dive"];
        }

        public BaseState Controlling(){
            return _states["control"];
        }

    #endregion

}
