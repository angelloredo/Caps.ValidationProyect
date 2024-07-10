import vueAwesomeSidebar from "vue-awesome-sidebar";
import "vue-awesome-sidebar/dist/vue-awesome-sidebar.css";
export default defineNuxtPlugin((nuxtApp) => {
  nuxtApp.vueApp.use(vueAwesomeSidebar);
});