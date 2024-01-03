using CommandQueues.Core;
using UnityEngine;

namespace ScenesLoaderSystem.Sample
{
    public class WaitTimeNodeCommandInstaller : NodeCommandInstaller
    {
        protected override INodeCommand GetNodeCommand()
        {
            Debug.Log("1 MonoInstaller");
            return new WaitTimeNodeCommand();
        }
    }
}