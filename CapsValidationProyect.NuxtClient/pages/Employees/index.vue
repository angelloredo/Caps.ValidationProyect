<template>
    <div >
        <EmployeeList></EmployeeList>
    </div>
</template>


<script setup lang="ts">

import EmployeeList from '~/components/Employees/employee-list-component.vue'
import { ref } from 'vue';

const runtimeConfig = useRuntimeConfig()
const publicVar = ref(runtimeConfig.public.myPublicVariable);

</script>