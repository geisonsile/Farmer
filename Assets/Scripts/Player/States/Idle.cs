using UnityEngine;

public class Idle : State {

    private PlayerController controller;
    
    public Idle(PlayerController controller) : base("Idle") {
        this.controller = controller;
    }

    public override void Enter() {
        base.Enter();
    }

    public override void Exit() {
        base.Exit();
    }

    public override void Update() {
        base.Update();

        // Switch to Jump
        if(controller.hasJumpInput) {
            controller.stateMachine.ChangeState(controller.jumpState);
            return;
        }

        // Switch to Walking
        if(!controller.movementVector.IsZero()) {
            controller.stateMachine.ChangeState(controller.walkingState);
            /*if (!controller.isCarry)
                controller.stateMachine.ChangeState(controller.walkingState);
            else
                controller.stateMachine.ChangeState(controller.carryState);*/

            return;
        }
    }

    public override void LateUpdate() {
        base.LateUpdate();
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
    }

}