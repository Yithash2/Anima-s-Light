

using System.Collections.Generic;

public class StateFactory
{
    PlayerStateMachine _context;

    public StateFactory(PlayerStateMachine currentContext){
        _context = currentContext;
    }

    public BaseState Grounded(){
        return new GroundState(_context, this);
    }

    public JumpState Jumping(){
        return new JumpState(_context, this);
    }

    public FallState Falling(){
        return new FallState(_context, this);
    }

    public RunState Running(){
        return new RunState(_context, this);
    }

    public IddleState Iddle(){
        return new IddleState(_context, this);
    }

    public WalledState Walled(){
        return new WalledState(_context, this);
    }

    public WallJumpState WallJump(){
        return new WallJumpState(_context, this);
    }

    public GroundSlamState GroundSlam(){
        return new GroundSlamState(_context, this);
    }
}
