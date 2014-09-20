using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JsonVu.Json.Tests {
    public class ExpectedExceptionWithMessageAttribute : ExpectedExceptionBaseAttribute {

        public ExpectedExceptionWithMessageAttribute(Type exceptionType,
            Type resourceType, string propertyName, params object[] values) {
            
            this.ExceptionType = exceptionType;
            var message = (string)resourceType.GetProperty(propertyName, 
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue(null);
            this.ExceptionMessage = string.Format(message, values);
        }

        public Type ExceptionType { get; private set; }

        public string ExceptionMessage { get; private set; }

        public bool AllowDerivedTypes { get; set; }

        protected override void Verify(Exception exception)
        {
            Type type = exception.GetType();
            if (this.AllowDerivedTypes) {
                if (!this.ExceptionType.IsAssignableFrom(type)) {
                    base.RethrowIfAssertException(exception);
                    Assert.IsInstanceOfType(exception, this.ExceptionType);
                }
            } else {
                if (type != this.ExceptionType) {
                    base.RethrowIfAssertException(exception);
                    Assert.AreEqual(type, this.ExceptionType);
                }
            }
            Assert.AreEqual(this.ExceptionMessage, exception.Message);
        }
    }
}
