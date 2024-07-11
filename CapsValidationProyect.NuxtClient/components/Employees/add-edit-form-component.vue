<template>
  <div>
    <div class="reservoirs-form.col">

      <div class="text-center" v-if="Spinner">
        <div class="spinner-border" role="status">
          <span class="sr-only"></span>
        </div>
      </div>

      <small class="text-muted"></small>
      <VForm v-if="!Spinner" >
        <div class="row">
          <div class="col-lg-4">
            <div class="for-group">
              <label>Primer nombre</label>
              <VField label="" name="" :rules="'required'" v-model="EmployeeDTO.firstName">
                <input type="text" v-model="EmployeeDTO.firstName" class="form-control">
              </VField>
              <div class="text-danger" style="font-size:small">
                <VErrorMessage label="" name="" v-slot="{ message }">
                  <i class="bi bi-exclamation-circle"></i> {{ message }}
                </VErrorMessage>
              </div>
            </div>

          </div>

          <div class="col-lg-4">
            <div class="for-group">
              <label>Segundo nombre</label>
              <VField label="" name="" :rules="'required'" v-model="EmployeeDTO.middleName">
                <input type="text" v-model="EmployeeDTO.middleName" class="form-control">
              </VField>
              <div class="text-danger" style="font-size:small">
                <VErrorMessage label="" name="" v-slot="{ message }">
                  <i class="bi bi-exclamation-circle"></i> {{ message }}
                </VErrorMessage>
              </div>
            </div>

          </div>

          <div class="col-lg-4">
            <div class="for-group">
              <label>Apellido</label>
              <VField label="" name="" :rules="'required'" v-model="EmployeeDTO.lastName">
                <input type="text" v-model="EmployeeDTO.lastName" class="form-control">
              </VField>
              <div class="text-danger" style="font-size:small">
                <VErrorMessage label="" name="" v-slot="{ message }">
                  <i class="bi bi-exclamation-circle"></i> {{ message }}
                </VErrorMessage>
              </div>
            </div>

          </div>

          <div class="col-lg-4">
            <div class="for-group">
              <label>Apellido materno</label>
              <VField label="" name="" :rules="'required'" v-model="EmployeeDTO.mothersLastName">
                <input type="text" v-model="EmployeeDTO.mothersLastName" class="form-control">
              </VField>
              <div class="text-danger" style="font-size:small">
                <VErrorMessage label="" name="" v-slot="{ message }">
                  <i class="bi bi-exclamation-circle"></i> {{ message }}
                </VErrorMessage>
              </div>
            </div>

          </div>




          <div class="col-lg-4">
            <label>Tipo</label>
            <VField label="" name="" v-model="EmployeeDTO.departmentId" :rules="'required'">
              <select class="form-control" v-model="EmployeeDTO.departmentId" placeholder="Seleccionar">
                <option :value="null">Seleccionar</option>
                <option v-for="tipo in EmployeeDepartments" :value="tipo.Id">
                  {{ tipo.Name }}
                </option>
              </select>
            </VField>
            <div class="text-danger" style="font-size:small">
              <VErrorMessage label="Tipo" name="Type" v-slot="{ message }">
                <i class="bi bi-exclamation-circle"></i> {{ message }}
              </VErrorMessage>
            </div>
          </div>
        </div>


        <div class="form-group">
          <br>
          <button @click.native="router.push('/Employees');" class="btn btn-danger btn-md" title="Cancelar"
            type="button" data-v-fe2a6b4c="">Cancelar</button>
          <button type="button" @click="EmployeeFormSubmit" class="btn btn-success float-right">
            <span v-if="loading">Guardando...</span>
            <span v-else>Guardar cambios</span>
          </button>
        </div>
      </VForm>
    </div>
  </div>
</template>

<script lang="ts" setup>
import { ref, onBeforeMount, watch, computed, type ComputedRef } from 'vue';
import { EmployeeModel, type IEmployeeModel } from '~/Entities/Employee/EmployeeModel';


const $storeItem = useEmployeeStore();

const router = useRouter();

const props = defineProps({
  isUpdate: { type: Boolean, required: false, default: false },
  EmployeePropId: { type: Number, required: false, default: false }
});


const EmployeeDTO = ref<IEmployeeModel>(new EmployeeModel());

const EmployeeDepartments = ref<{ Id: number; Name: string }[]>([
  { Id: 2, Name: 'Calidad' },
  { Id: 3, Name: 'Mtto' }
]);

const loading = ref<boolean>(false);
const Spinner = ref<boolean>(true);


const emits = defineEmits(['close']);
const EmployeeFormSubmit = async () => {
  
  loading.value = true;
  if (props.isUpdate) {
    await $storeItem.actualizar(EmployeeDTO.value);

  } else await $storeItem.agregar(EmployeeDTO.value);
 
  loading.value = false;
  event?.preventDefault();
  // resetForm();
}



const GetEmployeeByIdToUpdate = async () => {
  if (props.isUpdate) {
    

    const EmployeeById = await $storeItem.getById(props.EmployeePropId);
    EmployeeDTO.value = EmployeeById as IEmployeeModel;

  }
}
const ItemName: ComputedRef<string> = computed((): string => {

  return EmployeeDTO.value.fullName;
});


onBeforeMount(async () => {

  await GetEmployeeByIdToUpdate();
  Spinner.value = false;
});


</script>