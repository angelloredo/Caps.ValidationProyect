<template>

  <div style="display: flex;">

    <VueAwesomeSideBar v-model:miniMenu="miniMenu" v-model:collapsed="collapsed" :menu="testMenu" :position="('')" menuType="fully"
      vueRouterEnabel>
      <template #header><div align="center">CAPS</div>  </template>
    </VueAwesomeSideBar>


    <div style="padding: 20px;">
      <slot></slot>
    </div>
  </div>





</template>

<script setup>
import { ref } from 'vue'

const collapsed = ref(false)
const miniMenu = ref(false)

const testMenu = [
  {
    name: 'Getting Started',
    icon: { text: 'home', class: 'bi bi-house' },
    children: [
      {
        name: 'Empleados',
        href: '/Employees',
        icon: { text: 'empleados', class: 'bi bi-person-vcard' }
      },
      {
        name: 'Departamentos',
        href: '/Departments',
        icon: { text: 'Departamentos', class: 'bi bi-people-fill' }
      }
    ],
  }
]
</script>