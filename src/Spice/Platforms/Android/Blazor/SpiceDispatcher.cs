using Android.OS;

using Microsoft.AspNetCore.Components;

namespace Spice;

internal partial class SpiceDispatcher : Dispatcher
{
	readonly Looper _looper;
	readonly Handler _handler;

	public SpiceDispatcher()
	{
		_looper = Looper.MainLooper ?? throw new ArgumentNullException("Android.OS.Looper.MainLooper");
		_handler = new Handler(_looper);
	}

	public override bool CheckAccess() => _looper == Looper.MyLooper();

	public override Task InvokeAsync(Action workItem)
	{
		var tcs = new TaskCompletionSource<object>();

		_handler.Post(() =>
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

		_handler.Post(async () =>
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

		_handler.Post(() =>
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

		_handler.Post(async () =>
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
