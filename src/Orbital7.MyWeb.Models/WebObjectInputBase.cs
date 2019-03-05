using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.MyWeb.Models
{
    public abstract class WebObjectInputBase<T>
        where T : WebObjectBase
    {
        public string WebKey { get; set; }

        public Guid Id { get; set; }

        public WebObjectInputBase()
        {

        }

        public WebObjectInputBase(
            string webKey)
            : this()
        {
            this.WebKey = webKey;
        }

        public WebObjectInputBase(
            string webKey,
            T webObject)
            : this(webKey)
        {
            this.Id = webObject.Id;
        }

        public T Create(Web web)
        {
            var webObject = typeof(T).CreateInstance<T>();
            webObject.Web = web;

            if (this.Id != Guid.Empty)
                webObject.Id = this.Id;

            return Update(webObject);
        }

        public T Update(
            T webObject)
        {
            if (this.Id != Guid.Empty && this.Id != webObject.Id)
                throw new Exception("Input object and existing object have different Ids");

            UpdateProperties(webObject);

            return webObject;
        }

        protected abstract void UpdateProperties(
            T webObject);
    }
}
