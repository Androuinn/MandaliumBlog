
export interface MethodCallResponse<T>  {
    entity: T;
    message: string;
    statusCode: number;
}
