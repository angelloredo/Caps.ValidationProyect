
export interface IEmployeeModel {
    id: number;
    departmentId: number | null;
    fullName: string;
    firstName: string;
    middleName: string;
    lastName: string;
    mothersLastName: string;
    department: string;
  }

export class EmployeeModel implements IEmployeeModel {
    id: number;
    departmentId: number | null;
    fullName: string;
    firstName: string;
    middleName: string;
    lastName: string;
    mothersLastName: string;
    department: string = '';
  
    constructor(
      id: number = 0,
      departmentId: number | null = null,
      fullName: string = '',
      firstName: string = '',
      middleName: string = '',
      lastName: string = '',
      mothersLastName: string = ''
    ) {
      this.id = id;
      this.departmentId = departmentId;
      this.fullName = fullName;
      this.firstName = firstName;
      this.middleName = middleName;
      this.lastName = lastName;
      this.mothersLastName = mothersLastName;
    }
  }