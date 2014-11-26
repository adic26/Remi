//http://www.codeproject.com/Articles/57760/Exposing-Object-Methods-in-the-PropertyGrid-Comman

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Reflection;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Contains functionality to expose public methods of any object as <see cref="System.Windows.Forms.PropertyGrid"/> commands.
    /// Methods to expose must be decorated with the <see cref="BrowsableAttribute"/>.
    /// </summary>
    public class DesignerVerbSite : IMenuCommandService, ISite
    {
        private readonly object _component;

        /// <summary>
        /// Initialize a DesignerVerbSite to expose the public methods of the specified object.
        /// </summary>
        /// <param name="component">Object to wrap in the DesignerVerbSite</param>
        public DesignerVerbSite(object component)
        {
            _component = component;
        }

        /// <summary>
        /// Gets the service object of the <see cref="IMenuCommandService"/> type.
        /// </summary>
        /// <param name="serviceType">An <see cref="IMenuCommandService"/> object that specifies the type of service object to get.</param>
        /// <returns> A service object of type <see cref="IMenuCommandService"/>.</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof (IMenuCommandService))
                return this;
            return null;
        }

        /// <summary>
        /// Gets the collection of verbs that represent the methods on the component.
        /// </summary>
        public DesignerVerbCollection Verbs
        {
            get
            {
                DesignerVerbCollection verbs = new DesignerVerbCollection();
                // Use reflection to enumerate all the public methods on the object
                MethodInfo[] mia = _component.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (MethodInfo mi in mia)
                {
                    // Ignore any methods without a [Browsable(true)] attribute
                    object[] attrs = mi.GetCustomAttributes(typeof(BrowsableAttribute), true);
                    if (attrs.Length == 0)
                        continue;
                    if (!((BrowsableAttribute)attrs[0]).Browsable)
                        continue;
                    // Add a DesignerVerb with our VerbEventHandler
                    // The method name will appear in the command pane
                    verbs.Add(new DesignerVerb(mi.Name, VerbEventHandler));
                }
                return verbs;
            }
        }

        private void VerbEventHandler(object sender, EventArgs e)
        {
            // The verb is the sender
            DesignerVerb verb = (DesignerVerb)sender;
            // Enumerate the methods again to find the one named by the verb
            MethodInfo[] mia = _component.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
            foreach (MethodInfo mi in mia)
            {
                object[] attrs = mi.GetCustomAttributes(typeof(BrowsableAttribute), true);
                if (attrs.Length == 0 || !((BrowsableAttribute)attrs[0]).Browsable)
                    continue;
                //if (!((BrowsableAttribute)attrs[0]).Browsable)
                //    continue;
                if (verb.Text == mi.Name)
                {
                    // Invoke the method on our object (no parameters)
                    mi.Invoke(_component, null);
                    return;
                }
            }
        }

        #region ISite unused members

        /// <summary>
        /// Not implemented.
        /// </summary>
        public IComponent Component
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public IContainer Container
        {
            get { return null; }
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public bool DesignMode
        {
            get { return true; }
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public string Name
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region IMenuCommandService unused members

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="command">Not implemented.</param>
        public void AddCommand(MenuCommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="verb">Not implemented.</param>
        public void AddVerb(DesignerVerb verb)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="commandId">Not implemented.</param>
        /// <returns>Not implemented.</returns>
        public MenuCommand FindCommand(CommandID commandId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="commandId">Not implemented.</param>
        /// <returns>Not implemented.</returns>
        public bool GlobalInvoke(CommandID commandId)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="command">Not implemented.</param>
        public void RemoveCommand(MenuCommand command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="verb">Not implemented.</param>
        public void RemoveVerb(DesignerVerb verb)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="menuId">Not implemented.</param>
        /// <param name="x">Not implemented.</param>
        /// <param name="y">Not implemented.</param>
        public void ShowContextMenu(CommandID menuId, int x, int y)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}