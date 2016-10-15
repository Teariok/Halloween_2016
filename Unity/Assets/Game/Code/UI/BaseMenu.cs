using UnityEngine;
using System.Collections;

namespace Teario.Util
{
	public abstract class BaseMenu : MonoBehaviour
	{
		public virtual void OnPreEnter(){}
		public virtual void OnPostEnter(){}
        public virtual void OnPreExit( System.Action lCallback ){ lCallback(); }
		public virtual void OnPostExit(){}
	}
}