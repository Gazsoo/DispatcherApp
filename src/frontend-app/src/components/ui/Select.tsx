import type { SelectHTMLAttributes, ReactNode } from 'react';

interface SelectProps extends SelectHTMLAttributes<HTMLSelectElement> {
    label?: string;
    children?: ReactNode;
}

export const Select = ({ label, id, className = '', children, ...props }: SelectProps) => {
    return (
        <div>
            {label && (
                <label htmlFor={id} className="block text-sm font-medium mb-2">
                    {label}
                </label>
            )}
            <select
                id={id}
                className={`w-full px-4 py-3 rounded-lg bg-surface-light dark:bg-surface-dark border border-surface-light-border dark:border-surface-dark-border focus:outline-none focus:ring-2 focus:ring-accent transition-all ${className}`}
                {...props}
            >
                {children}
            </select>
        </div>
    );
};
