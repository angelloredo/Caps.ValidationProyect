<template>
    <div style="padding: 20px;">
      <InvItemDetailComponent v-if="ItemId > 0" ref="invItemDetails" :EmployeePropId="ItemId">
      </InvItemDetailComponent>
    </div>
  </template>
     
  <script setup lang="ts">
  import { ref, onBeforeMount, watch } from 'vue';
  
  import { useRouter, useRoute } from "vue-router";
  import InvItemDetailComponent from '@/components/Employees/details-component.vue';
  const route = useRoute();
  const router = useRouter();
  const isUpdate = ref(false);
  const ItemId = ref(0);
  
  onBeforeMount(async () => {
    ItemId.value = route.query.Id ? parseInt(route.query.Id.toString()) : 0;
  
  });
  </script>