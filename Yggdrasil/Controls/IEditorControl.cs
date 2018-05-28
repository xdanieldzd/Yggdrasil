using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Controls
{
	interface IEditorControl
	{
		bool IsInitialized();
		bool IsBusy();
		void Initialize(GameDataManager gameDataManager);
		void Rebuild();
		void Terminate();
	}
}
