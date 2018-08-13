using UnityEngine;
using Stratus;
using System.Text;

namespace Stratus
{
  namespace AI
  {
    public abstract class BehaviorSystem : StratusScriptable
    {
      //------------------------------------------------------------------------/
      // Fields
      //------------------------------------------------------------------------/
      [Header("Debug")]
      /// <summary>
      /// Whether this system will print debug output
      /// </summary>
      public bool debug = false;

      /// <summary>
      /// A short description of what this system does
      /// </summary>
      public string description;

      /// <summary>
      /// The blackboard the blackboard is using.
      /// </summary>
      public Blackboard blackboard;

      //------------------------------------------------------------------------/
      // Properties
      //------------------------------------------------------------------------/
      /// <summary>
      /// Whether this behavior system is currently running
      /// </summary>
      public bool active { protected set; get; }

      /// <summary>
      /// The agent this system is using
      /// </summary>
      public Agent agent { private set; get; }

      /// <summary>
      /// The sensor the agent is using
      /// </summary>
      protected Sensor sensor { private set; get; }

      /// <summary>
      /// The current behavior being run by this system
      /// </summary>
      protected Behavior currentBehavior { set; get; }

      //----------------------------------------------------------------------/
      // Properties: Static
      //----------------------------------------------------------------------/


      //------------------------------------------------------------------------/
      // Interface
      //------------------------------------------------------------------------/
      protected abstract void OnStart();
      protected abstract void OnSubscribe();
      protected abstract void OnUpdate(float dt);
      protected abstract void OnPrint(StringBuilder builder);
      // Behaviors
      public abstract void OnBehaviorStarted(Behavior behavior);
      public abstract void OnBehaviorEnded(Behavior behavior);
      public abstract void OnAssess();

      //------------------------------------------------------------------------/
      // Methods
      //------------------------------------------------------------------------/
      /// <summary>
      /// Returns an instance of this behavior system to be used by a
      /// single agent.
      /// </summary>
      /// <param name="agent"></param>
      /// <returns></returns>
      public T Instantiate<T>(Agent agent) where T : BehaviorSystem
      {
        var instance = Instantiate(this) as T;
        instance.agent = agent;
        instance.sensor = agent.sensor;
        return instance;
      }

      /// <summary>
      /// Configures the system. In order to initialize behaviors call Assesss.
      /// </summary>
      public void Initialize()
      {
        this.Subscribe();
        this.OnStart();
      }

      /// <summary>
      /// Updates this behavior system.
      /// </summary>
      /// <param name="dt"></param>
      public void UpdateSystem(float dt)
      {
        this.OnUpdate(dt);
      }

      /// <summary>
      /// Assess the situation, coming up with the next action for the agent to take
      /// </summary>
      public void Assess()
      {
        this.OnAssess();
      }

      /// <summary>
      /// Prints a representation of the contents of this behavior system
      /// </summary>
      /// <returns></returns>
      public string Print()
      {
        var builder = new StringBuilder();
        OnPrint(builder);
        return builder.ToString();
      }
      
      /// <summary>
      /// Cancels the agent's current action, forcing it to reassess the situation
      /// </summary>
      public void Cancel()
      {
        this.Assess();
      }

      //------------------------------------------------------------------------/
      // Methods
      //------------------------------------------------------------------------/
      /// <summary>
      /// Subscribe the agent to specific events to the system
      /// </summary>
      void Subscribe()
      {
        this.OnSubscribe();
      }

    }
  }

}