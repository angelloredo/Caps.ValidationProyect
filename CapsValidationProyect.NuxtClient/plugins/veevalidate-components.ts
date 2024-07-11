import { Form, Field, ErrorMessage } from "vee-validate";

export default defineNuxtPlugin((nuxtApp) => {
  nuxtApp.vueApp.component("VForm", Form);
  nuxtApp.vueApp.component("VField", Field);
  nuxtApp.vueApp.component("VErrorMessage", ErrorMessage);
});

declare module "#app" {
  interface NuxtApp {
    VForm: typeof Form;
    VField: typeof Field;
    VErrorMessage: typeof ErrorMessage;
  }
}
declare module "vue" {
  interface ComponentCustomProperties {
    VForm: typeof Form;
    VField: typeof Field;
    VErrorMessage: typeof ErrorMessage;
  }
}
