export function assertRequired<T>(
    value: T | undefined | null, 
    message: string = 'Required value is missing'
): asserts value is T {
    if (value == null || (typeof value === 'string' && !value.trim())) {
        throw new Error(message);
    }
}

export function assertRequiredFields<T extends Record<string, any>>(
    obj: T,
    fields: (keyof T)[],
    prefix: string = 'Missing required field'
): asserts obj is Required<Pick<T, keyof T>> {
    fields.forEach(field => {
        assertRequired(obj[field], `${prefix}: ${String(field)}`);
    });
}