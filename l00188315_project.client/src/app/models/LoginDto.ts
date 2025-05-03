export interface LoginDto {
  email:any,
  password:any
}
export interface LoginResponse {
  token:string
}

export interface RegisterDto {
  email:any,
  password:any
  name:any
}
