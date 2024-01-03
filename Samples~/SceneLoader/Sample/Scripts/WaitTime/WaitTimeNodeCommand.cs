using System.Threading.Tasks;
using CommandQueues.Core;
using UnityEngine;

namespace ScenesLoaderSystem.Sample
{
    public class WaitTimeNodeCommand : NodeCommand
    {
        public async override void Execute()
        {
            Debug.Log("Scene Loaded");

            await Task.Delay(1000);

            Debug.Log("Load Done");

            NotifyDoneExecution();
        }
    }
}