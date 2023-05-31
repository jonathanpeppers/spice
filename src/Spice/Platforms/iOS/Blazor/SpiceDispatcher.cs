using System.Reflection.Metadata;

using CoreFoundation;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Spice;

internal partial class SpiceDispatcher : Dispatcher
{
	readonly DispatchQueue _dispatchQueue;

	public SpiceDispatcher()
	{
		_dispatchQueue = DispatchQueue.MainQueue;
	}

	public override bool CheckAccess() => DispatchQueue.CurrentQueueLabel == _dispatchQueue.Label;

	public override Task InvokeAsync(Action workItem)
	{
		var tcs = new TaskCompletionSource<object>();

		_dispatchQueue.DispatchAsync(() =>
		{
			try
			{
				workItem();
				tcs.SetResult(new object());
			}
			catch (Exception ex)
			{
				tcs.SetException(ex);
			}
		});

		return tcs.Task;
	}

	public override Task InvokeAsync(Func<Task> workItem)
	{
		var tcs = new TaskCompletionSource<object>();

		_dispatchQueue.DispatchAsync(async () =>
		{
			try
			{
				await workItem();
				tcs.SetResult(new object());
			}
			catch (Exception ex)
			{
				tcs.SetException(ex);
			}
		});

		return tcs.Task;
	}

	public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
	{
		var tcs = new TaskCompletionSource<TResult>();

		_dispatchQueue.DispatchAsync(() =>
		{
			try
			{
				tcs.SetResult(workItem());
			}
			catch (Exception ex)
			{
				tcs.SetException(ex);
			}
		});

		return tcs.Task;
	}

	public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
	{
		var tcs = new TaskCompletionSource<TResult>();

		_dispatchQueue.DispatchAsync(async () =>
		{
			try
			{
				tcs.SetResult(await workItem());
			}
			catch (Exception ex)
			{
				tcs.SetException(ex);
			}
		});

		return tcs.Task;
	}
}

