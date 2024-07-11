export interface IPaginationModel {
    recordList: Array<{ [key: string]: any }>;
    totalRecords: number;
    totalPages: number;
  }

  export class PaginationModel implements IPaginationModel {
    recordList: Array<{ [key: string]: any }>;
    totalRecords: number;
    totalPages: number;
  
    constructor(
      recordList: Array<{ [key: string]: any }> = [],
      totalRecords: number = 0,
      totalPages: number = 0
    ) {
      this.recordList = recordList;
      this.totalRecords = totalRecords;
      this.totalPages = totalPages;
    }
  }