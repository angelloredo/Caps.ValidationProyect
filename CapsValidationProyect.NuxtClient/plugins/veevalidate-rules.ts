import { defineRule, configure } from 'vee-validate'

import { localize, setLocale } from '@vee-validate/i18n'

export default defineNuxtPlugin(() => {
  // defineRule('required', required);
  // defineRule('email', email);
  // defineRule('min', min);


  defineRule('itemname', (value: any, target: string) => {
    // Field is empty, should pass
    if (target[0] === 'false') {
      return true;
    }
    return 'Ya existe un Artículo con ese nombre';
  });

  defineRule('curpvalidate', (value: any, target: string) => {
    // Field is empty, should pass
    if (value.length == 0 || value.length > 0 && value.length  >= 18) {
      return true;
    }
    return 'La CURP no es valida';
  });




  setLocale('es')
})
