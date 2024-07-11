<template>
    <div>
      
        <div class="row" v-if="loadingData" style="justify-content: center;">
            <div class="spinner-border" role="status">
                <span class="sr-only"></span>
            </div>
        </div>
        <div v-else class="reservoirs-form.col">
            <div class="modal-header">
                <span class="h5 font-bold center">Detalles</span>
            </div>

            <div class="row">
                <div class="col-lg-10">
                    <div class="d-inline-block">
                        <button class="btn btn-primary"
                            @click="router.push('/Employees/Edit?Id=' + props.EmployeePropId);">
                            <i class="bi bi-pencil-square"></i>
                        </button>

                    </div>
                    <div class="mx-1 d-inline-block">
                        <modal modal-id="deleteModal">
                            <template #button>
                                <i class="bi bi-trash-fill"></i>
                            </template>
                            <template #content>
                                <div class="container">
                                    <div class="row">
                                        <div class="modal-title">
                                            <h5>
                                                ¿Desea eliminar el empleado {{ EmployeeDTO.fullName }}?.
                                            </h5>
                                        </div>
                                        <div class="modal-body">
                                            <p>Por favor confirme para eliminar el elemento seleccionado.</p>
                                        </div>
                                        <div class="modal-footer">
                                            <button class="btn btn-outline-danger btn-sm" data-bs-dismiss="modal"><i
                                                    class="bi bi-x-circle"></i>
                                            </button>
                                            <button type="button" class="btn btn-danger btn-sm"
                                                @click="DeleteEmployeeElement()">
                                                Eliminar
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </template>
                            <!-- @deleteArea="DeleteModal" -->
                        </modal>
                    </div>

                </div>
               



            </div>

            <div class="modal-body">
                <small class="text-muted"></small>
                <VForm @submit="EmployeeFormSubmit">

                    <div class="row">
                        <div class="col-lg-4">
                            <div class="for-group">
                                <label>Primer nombre</label>
                                <h5>{{ EmployeeDTO.firstName }}</h5>
                            </div>

                        </div>

                        <div class="col-lg-4">
                            <label>Segundo nombre </label>
                            <h5>{{ EmployeeDTO.middleName }}</h5>
                        </div>

                        <div class="col-lg-4">
                            <label>Apellido </label>
                            <h5>{{ EmployeeDTO.lastName }}</h5>
                        </div>

                        <div class="col-lg-4">
                            <label>Apellido materno</label>
                            <h5>{{ EmployeeDTO.mothersLastName }}</h5>
                        </div>

                        <div class="col-lg-4">
                            <label>Departamento</label>
                            <h5>{{ EmployeeDTO.department }}</h5>
                        </div>

                    </div>


                   
                    <div class="form-group">
                        <button @click.native="backHistory" class="btn btn-primary btn-md" title="Cancelar"
                            type="button" data-v-fe2a6b4c="">Regresar
                        </button>
                    </div>
                </VForm>
            </div>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ref, onBeforeMount, watch } from 'vue';
import Modal from '~/components/modal.vue'


import { EmployeeModel, type IEmployeeModel } from '~/Entities/Employee/EmployeeModel';
const router = useRouter();



const EmpleadoStore = useEmployeeStore();
const { eliminar, agregar, actualizar, EmployeeActual } = EmpleadoStore
const { employees } = storeToRefs(EmpleadoStore);

const backHistory = () => {
    window.history.go(-1);
}


const props = defineProps({
    isUpdate: { type: Boolean, required: false, default: false },
    EmployeePropId: { type: Number, required: false, default: 0 }
});


const EmployeeDTO = ref<IEmployeeModel>(new EmployeeModel());
const EmployeeDepartments = ref<{ value: number; text: string }[]>([]);

const loading = ref<boolean>(false);
const loadingData = ref<boolean>(true);

const formatDateMX = (date: Date): string => {
    if (date !== null) {
        return date.toLocaleDateString('es-Mx');

    } else { return ''; }
};

const EmployeeFormSubmit = async () => {
    loading.value = true;
    if (props.isUpdate) {
        await actualizar(EmployeeDTO.value);
    } else await agregar(EmployeeDTO.value);
    loading.value = false;
    resetForm();
}
const emits = defineEmits(['close', 'edit']);


const DeleteEmployeeElement = () => {
    eliminar(props.EmployeePropId);
}

const resetForm = () => {
    EmployeeDTO.value = new EmployeeModel();
    emits('close');
}

const GetInvItemByIdDetails = async () => {
  loadingData.value = true;
  let res = await EmpleadoStore.getById(props.EmployeePropId);
  

  EmployeeDTO.value = res as IEmployeeModel;
  loadingData.value = false;


}




onBeforeMount(async () => {
    await GetInvItemByIdDetails();
});





</script>