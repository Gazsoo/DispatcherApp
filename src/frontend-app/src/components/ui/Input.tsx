import type{ InputHTMLAttributes } from 'react';

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string;
}

export const Input = ({ label, id, className = '', ...props }: InputProps) => {
  return (
    <div>
      {label && (
        <label htmlFor={id} className="block text-sm font-medium mb-2">
          {label}
        </label>
      )}
      <input
        id={id}
        className={`w-full px-4 py-3 rounded-lg bg-surface-light dark:bg-surface-dark border border-surface-light-border dark:border-surface-dark-border focus:outline-none focus:ring-2 focus:ring-accent transition-all ${className}`}
        {...props}
      />
    </div>
  );
};
