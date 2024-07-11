<template>
    <div style="padding: 20px;">
        <employeFormComponent ref="employeeForm" :isUpdate="false" :EmployeePropId="0"
                ></employeFormComponent>
    </div>
  </template>
   
  <script setup lang="ts">
  import employeFormComponent from '@/components/Employees/add-edit-form-component.vue';
  
  </script>