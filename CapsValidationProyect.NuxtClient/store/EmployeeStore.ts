import { defineStore } from "pinia";
import type { IEmployeeModel } from "~/Entities/Employee/EmployeeModel";
import { PaginationModel, type IPaginationModel } from "~/Entities/PaginationModel";
import { toast } from 'vue3-toastify';
import { useRouter } from 'vue-router';

interface IState {
    employees: IEmployeeModel[],
    employeesPagination: IPaginationModel,
    EmployeeActual: IEmployeeModel | null,
    isLoading: boolean,
    accessToken: string | null // Token JWT
}

const runtimeConfig = useRuntimeConfig();
const defaultUrl = ref(runtimeConfig.public.myPublicVariable);
const router = useRouter();

export const useEmployeeStore = defineStore('employees', {
    state: (): IState => ({
        employees: [],
        employeesPagination: new PaginationModel(),
        EmployeeActual: null,
        isLoading: false,
        accessToken: "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJhbmdlbGxvcmVkbyIsIm5iZiI6MTcyMDY2OTQ3NiwiZXhwIjoxNzIzMjYxNDc2LCJpYXQiOjE3MjA2Njk0NzZ9.oXOY7-nMihu3nJkvBtWxoq_RGogOYed3zye8KMccF8Yiq9XyYkLzumAhwjmdkG-kR-xALCo1k7-D3lit71g4_Q" // Initialize with null
    }),
    actions: {
        // Método para establecer el token JWT después del inicio de sesión
        setAccessToken(token: string) {
            this.accessToken = token;
        },

        // Métodos para realizar solicitudes con el token JWT
        async getById(id: number) {
            const { data, error } = await this.fetchWithToken(`${defaultUrl.value}/api/Employee/${id}`, {
                method: 'GET'
            });
            if (error.value) {
                console.error("Error fetching employee:", error.value);
                toast.error("Error al obtener el empleado.");
                return;
            }

            this.EmployeeActual = data.value as IEmployeeModel;
            return data.value as IEmployeeModel;
        },
        async obtenerEmployeesPagination(pageNumber: number, pageSize: number = 0) {
            this.isLoading = true;
            const { data, error } = await this.fetchWithToken(defaultUrl.value + '/api/Employee/GetEmployeePagination', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    Name: "",
                    NumeroPagina: pageNumber,
                    CantidadElementos: pageSize
                })
            });
            if (error.value) {
                console.error("Error fetching employees:", error.value);
                toast.error("Error al obtener la lista de empleados.");
                this.isLoading = false;
                return;
            }
            this.employeesPagination = data.value as IPaginationModel ?? new PaginationModel();
            this.isLoading = false;
        },
        async obtenerEmployees() {
            const { data, error } = await this.fetchWithToken(defaultUrl.value + '/api/Employee', {
                method: 'GET'
            });
            if (error.value) {
                console.error("Error fetching employees:", error.value);
                toast.error("Error al obtener la lista de empleados.");
                return;
            }
            this.employees = data.value as IEmployeeModel[] ?? [];
        },
        async agregar(body: IEmployeeModel) {
            const { data, error } = await this.fetchWithToken(defaultUrl.value + '/api/Employee', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(body)
            });
            if (error.value) {
                console.error("Error adding employee:", error.value);
                this.handleValidationErrors(error.value);
                return;
            } else {
                // this.obtenerEmployees(); // Refetch the list after adding a new employee
                router.push('/Employees'); // Navigate to Employees page
                toast.success("Empleado guardado correctamente.");
            }

        },
        async actualizar(body: IEmployeeModel) {
            const { data, error } = await this.fetchWithToken(`${defaultUrl.value}/api/Employee/${body.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(body)
            });
            if (error.value) {
                console.error("Error updating employee:", error.value);
                this.handleValidationErrors(error.value);
                return;
            } else {
                // this.obtenerEmployees(); // Refetch the list after updating an employee
                router.push('/Employees'); // Navigate to Employees page
                toast.success("Empleado actualizado correctamente.");
            }
        },
        async eliminar(id: number) {
            const { data, error } = await this.fetchWithToken(`${defaultUrl.value}/api/Employee/${id}`, {
                method: 'DELETE'
            });
            if (error.value) {
                console.error("Error deleting employee:", error.value);
                toast.error("Error al eliminar el empleado.");
                return;
            } else {
                this.employeesPagination.recordList = this.employeesPagination.recordList.filter(x => x.id !== id);
                router.push('/Employees'); // Navigate to Employees page
                toast.success("Empleado eliminado correctamente.");
            }

        },
        setEmployeeActual(Employee: IEmployeeModel | null) {
            this.EmployeeActual = Employee;
        },
        handleValidationErrors(error: any) {
            
            if (error.statusCode === 400 && error.data?.errors) {
                const errors = error.data.errors;
                let errorMessage = "Errores de validación: ";
                Object.keys(errors).forEach(key => {
                    const messages = errors[key];
                    messages.forEach((message: string) => {
                        errorMessage += `\n${message}`;
                    });
                });
                toast.error(errorMessage, {
                    autoClose: false // Keeps the toast open until the user dismisses it
                });
            } else {
                toast.error("Ocurrió un error inesperado.");
            }
        },

        // Función para manejar las solicitudes con token JWT
        async fetchWithToken(url: string, options: any) {
            if (!this.accessToken) {
                toast.error("Error 401. Sin autorización.");
            }

            options.headers = {
                ...options.headers,
                Authorization: `Bearer ${this.accessToken}`
            };

            const response = await useFetch(url, options);

            // Manejo de errores, etc.
            return response;
        },

        // Resto de métodos y acciones ...
    }
});
