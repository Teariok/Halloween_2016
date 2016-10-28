using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Teario.Util
{
	public class MenuManager : MonoBehaviour
	{
		private Dictionary<Type,BaseMenu> m_AllMenus;
		private Stack<BaseMenu> m_Menus;

		void Awake()
		{
			m_Menus = new Stack<BaseMenu>();

			BaseMenu[] lMenus = GetComponentsInChildren<BaseMenu>( true );
			Debug.Assert( lMenus != null && lMenus.Length != 0, "No Menus Found!" );
			
			m_AllMenus = new Dictionary<Type, BaseMenu>( lMenus.Length );

			for( int i = 0; i < lMenus.Length; ++i )
			{
				m_AllMenus[ lMenus[i].GetType() ] = lMenus[i];
			}
		}

		public bool PushMenu( Type lTarget )
		{
			if( m_AllMenus.ContainsKey(lTarget) && (m_Menus.Count == 0 || m_Menus.Peek().GetType() != lTarget) )
			{
				BaseMenu lCurrent = m_Menus.Count == 0 ? null : m_Menus.Peek();
				BaseMenu lNext = m_AllMenus[lTarget];

                Action lPostExitHandler = () => {
                    lNext.OnPreEnter();

                    if( lCurrent != null )
                    {
                        lCurrent.gameObject.SetActive( false );
                    }
    
                    m_Menus.Push( lNext );
                    lNext.gameObject.SetActive( true );
                    
                    lNext.OnPostEnter();
                    if( lCurrent != null )
                    {
                        lCurrent.OnPostExit();
                    }
                };

				if( lCurrent != null )
				{
					lCurrent.OnPreExit( lPostExitHandler );
				}
                else
                {
                    lPostExitHandler();
                }

				return true;
			}

			return false;
		}

		public bool PopMenu()
		{
			if( m_Menus.Count > 0 )
			{
				BaseMenu lCurrent = m_Menus.Pop();
				BaseMenu lNext = m_Menus.Count == 0 ? null : m_Menus.Peek();

				Debug.Assert( lCurrent != null );

                Action lExitHandler = () => {
                    if( lNext != null )
                    {
                        lNext.OnPreEnter();
                    }
    
                    lCurrent.gameObject.SetActive( false );
    
                    if( lNext != null )
                    {
                        lNext.gameObject.SetActive( true );
                        lNext.OnPostEnter();
                    }
    
                    lCurrent.OnPostExit();
                };
	
				lCurrent.OnPreExit( lExitHandler );

				return true;
			}

			return false;
		}

        public void ClearStack()
        {
            if( m_Menus.Count > 0 )
            {
                BaseMenu lCurrent = m_Menus.Pop();

                Debug.Assert( lCurrent != null );
    
                lCurrent.OnPreExit( ()=>{} );
                lCurrent.gameObject.SetActive( false );
                lCurrent.OnPostExit();

                m_Menus.Clear();
            }
        }
	}
}