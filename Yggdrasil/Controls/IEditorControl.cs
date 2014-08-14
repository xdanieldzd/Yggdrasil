﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yggdrasil.Controls
{
    interface IEditorControl
    {
        bool IsInitialized();
        void Initialize(GameDataManager game);
        void Terminate();
    }
}
