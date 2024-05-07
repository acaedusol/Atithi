using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Atithi.Web.Services
{
    public class LowercaseControllerNameConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            // Set the controller's route in lowercase
            controller.ControllerName = controller.ControllerName.ToLower();

            foreach (var selector in controller.Selectors)
            {
                if (selector.AttributeRouteModel != null)
                {
                    var template = selector.AttributeRouteModel.Template;
                    selector.AttributeRouteModel.Template = template.ToLower(); // Convert route to lowercase
                }
            }
        }
    }

}
