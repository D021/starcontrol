/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

/**
 * @class strange.extensions.sequencer.impl.SequenceCommand
 * 
 * A subclass of Command that runs in the Sequencer
 * 
 * A SequenceCommand is simply a Command that runs in a series
 * and can optionally call `BreakSequence()` to stop the implementation
 * of that series.
 * 
 * @see strange.extensions.command.api.ICommand
 */ 

using System;
using strange.extensions.command.impl;
using strange.extensions.injector.api;
using strange.extensions.sequencer.api;

namespace strange.extensions.sequencer.impl
{
	public class SequenceCommand : ISequenceCommand
	{
		[Inject]
		public ISequencer sequencer{ get; set;}

		[Inject]
		public IInjectionBinder injectionBinder{ get; set;}
		
		public object data{ get; set; }

		private bool _retain = false;
		private bool _cancelled = false;

		public SequenceCommand ()
		{
		}

		public void BreakSequence ()
		{
			if (sequencer != null)
			{
				sequencer.BreakSequence (this);
			}
		}

		virtual public void Execute ()
		{
			throw new SequencerException ("You must override the Execute method in every SequenceCommand", SequencerExceptionType.EXECUTE_OVERRIDE);
		}

		public void Retain ()
		{
			_retain = true;
		}

		public void Release ()
		{
			_retain = false;
			if (sequencer != null)
			{
				sequencer.ReleaseCommand (this);
			}
		}

		public void Cancel ()
		{
			_cancelled = true;
		}

		public bool retain 
		{
			get 
			{
				return _retain;
			}
		}

		public bool cancelled 
		{
			get 
			{
				return _cancelled;
			}
		}

		public int sequenceId{ get; set; }
	}
}

