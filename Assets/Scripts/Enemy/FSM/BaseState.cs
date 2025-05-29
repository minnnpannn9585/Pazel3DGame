namespace Enemy.FSM
{
    public abstract class BaseState
    {
        protected BaseEnemy CurrentEnemy;
        public abstract void OnEnter(BaseEnemy enemy);
        public abstract void OnUpdate();
        public abstract void OnFixedUpdate();
        public abstract void OnExist();
        public virtual void OnCall(){}
    }
}
