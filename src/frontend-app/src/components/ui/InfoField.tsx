interface InfoFieldProps {
    label: string;
    value: string | number | null | undefined;
    placeholder?: string;
}

export function InfoField({ label, value, placeholder = 'Not set' }: InfoFieldProps) {
    return (
        <div>
            <div className="text-sm text-gray-500">{label}</div>
            <div className="text-lg">{value || placeholder}</div>
        </div>
    );
}