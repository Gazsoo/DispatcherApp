// components/ui/FormInput.tsx
interface FormInputProps {
    label: string;
    id: string;
    type?: string;
    value: string;
    onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
    disabled?: boolean;
    helperText?: string;
}

export function FormInput({
    label,
    id,
    type = "text",
    value,
    onChange,
    disabled = false,
    helperText
}: FormInputProps) {
    return (
        <div>
            <label
                htmlFor={id}
                className={`block mb-1 text-sm font-medium ${disabled ? 'text-gray-500 dark:text-gray-400' : ''
                    }`}
            >
                {label}
            </label>
            <input
                id={id}
                type={type}
                value={value}
                onChange={onChange}
                disabled={disabled}
                className={`
                    w-full px-3 py-2 rounded-lg border
                    focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent
                    dark:border-gray-600
                    transition-colors
                    ${disabled
                        ? 'bg-gray-100 text-gray-500 cursor-not-allowed dark:bg-gray-700 dark:text-gray-400'
                        : 'dark:bg-gray-700'
                    }
                `}
            />
            {helperText && (
                <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
                    {helperText}
                </p>
            )}
        </div>
    );
}