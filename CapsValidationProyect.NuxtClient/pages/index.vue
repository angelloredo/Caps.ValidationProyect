<template>
    <div>Pagina principal</div>
</template>


<script setup lang="ts">

import { ref } from 'vue';

const runtimeConfig = useRuntimeConfig()
const publicVar = ref(runtimeConfig.public.myPublicVariable);

</script>