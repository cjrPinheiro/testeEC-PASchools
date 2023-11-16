import { Address } from "./address";

export interface School {
  name : string;
  publicDepartment : boolean;
  educationType : string;
  code : number;
  phoneNumber : string;
  email : string;
  webSite : string;
  activeOrganization : boolean;
  distance : number;
  address : Address;
  distanceText: string;
}
