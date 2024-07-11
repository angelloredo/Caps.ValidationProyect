<template>
    <div class="container-lg">
        <div class="row" v-if="!Spinner">
            <div class="col">
                <h3>Empleados</h3>
            </div>
        </div>
        <div class="mb-3" v-if="!isShowModalDetails && !FormCreate && !Spinner">
            <button class="btn btn-success" @click.native="router.push('/Employees/Create');">Agregar Empleado</button>
        </div>

        <div>
            <div class="text-center" v-if="Spinner">

                <div class="spinner-border" role="status">
                    <span class="sr-only"></span>
                </div>
            </div>

            <div class="mt-3" v-if="Index && !isShowModalDetails && $store.employeesPagination.totalRecords > 0" align="center">

                <PaginationComponent :current-page="currentPage" :total-items="$store.employeesPagination.totalRecords"
                    :items-per-page="pageSize" @update:current-page="currentPage = $event" />

                   <span style="float: right;">Total: {{$store.employeesPagination.totalRecords}}</span>
            </div>

            <table v-if="Index && !isShowModalDetails" class="table table-bordered table-responsive table-hover">
                <thead class="text-bg-dark">
                    <tr>
                        <th>Nombre</th>
                        <th>Opciones</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-if="$store.isLoading">
                        <td align="center" colspan="4">
                            <div class="spinner-border" role="status">
                                <span class="sr-only"></span>
                            </div>
                        </td>

                    </tr>
                    <tr v-else-if="$store.employeesPagination.recordList.length > 0"
                        v-for="(item, index) in $store.employeesPagination.recordList" :key="index">
                        <td>{{ item.FullName }}</td>
                      
                        <td align="center">

                            <a @click="$event.preventDefault(); router.push('/Employees/Details?Id=' + item.Id);" title="Detalles" style="padding: 2px;"><i
                                    class="bi bi-info-circle-fill"></i></a>
                        </td>
                    </tr>
                    <tr v-else>
                        <td colspan="4">Sin resultados</td>
                    </tr>
                </tbody>
            </table>



        </div>
    </div>
</template>

<script lang="ts" setup>

import { ref, onBeforeMount, watch } from 'vue';
import PaginationComponent from '~/components/common/PaginationComponent.vue'


const EmpleadoStore = useEmployeeStore();
const { getById, obtenerEmployeesPagination, setEmployeeActual } = EmpleadoStore
const { employees } = storeToRefs(EmpleadoStore);

const router = useRouter();
const $store = useEmployeeStore();

const pageSize = ref<number>(5);
const currentPage = ref<number>(1);
const isShowModalDetails = ref<boolean>(false);
const Index = ref<boolean>(false);
const FormCreate = ref<boolean>(false);
const Spinner = ref<boolean>(true);



onBeforeMount(async () => {
    Spinner.value = true;
    Spinner.value = false;
    Index.value = true;
    await obtenerEmployeesPagination(currentPage.value, pageSize.value);
});

watch(currentPage, async (val: any | null, oldVal: any | null) => {
    if (val >= 1 && oldVal !== undefined) {
        await obtenerEmployeesPagination(currentPage.value, pageSize.value);
    }
});

</script>
