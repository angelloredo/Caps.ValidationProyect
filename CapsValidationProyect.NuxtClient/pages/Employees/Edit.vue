<template>
    <div style="padding: 20px;">
        <InvItemFormComponent2  v-if="ItemId > 0" ref="invItemForm" :isUpdate="true"
                :EmployeePropId="ItemId"></InvItemFormComponent2>
    </div>
  </template>
   
  <script setup lang="ts">
import { ref, onBeforeMount, watch } from 'vue';
  
  import { useRouter, useRoute } from "vue-router";
import InvItemFormComponent2 from '@/components/Employees/add-edit-form-component.vue';
const route = useRoute();
const ItemId = ref(0);
  
onBeforeMount(async () => {
    ItemId.value = route.query.Id ? parseInt(route.query.Id.toString()) : 0;
    
});
  </script>