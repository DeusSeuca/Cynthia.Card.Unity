using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class UnityTaskScheduler : TaskScheduler
{
	private readonly SynchronizationContext unitySynchronizationContext;
	private readonly LinkedList<Task> queue = new LinkedList<Task>();

	public UnityTaskScheduler(SynchronizationContext context) => unitySynchronizationContext = context;

	protected override IEnumerable<Task> GetScheduledTasks()
	{
		lock (queue)
		{
			return queue.ToArray();
		}
	}

	protected override void QueueTask(Task task)
	{
		lock (queue)
		{
			queue.AddLast(task);
		}
	}

	protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
	{
		if (SynchronizationContext.Current != unitySynchronizationContext)
		{
			return false;
		}

		if (taskWasPreviouslyQueued)
		{
			lock (queue)
			{
				queue.Remove(task);
			}
		}

		return TryExecuteTask(task);
	}

	public void ExecutePendingTasks()
	{
		while (true)
		{
			Task task;
			lock (queue)
			{
				if (queue.Count == 0)
				{
					break;
				}

				task = queue.First.Value;
				queue.RemoveFirst();
			}

			if (task != null)
			{
				var result = TryExecuteTask(task);
				if (result == false)
				{
					throw new InvalidOperationException();
				}
			}
		}
	}
}