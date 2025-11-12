import type { TextareaHTMLAttributes } from 'react';

interface TextAreaProps extends TextareaHTMLAttributes<HTMLTextAreaElement> {
  label?: string;
}

export const TextArea = ({
  label,
  id,
  className = '',
  rows = 4,
  ...props
}: TextAreaProps) => {
  return (
    <div>
      {label && (
        <label htmlFor={id} className="block text-sm font-medium mb-2">
          {label}
        </label>
      )}
      <textarea
        id={id}
        rows={rows}
        className={`w-full px-4 py-3 rounded-lg bg-surface-light dark:bg-surface-dark border border-surface-light-border dark:border-surface-dark-border focus:outline-none focus:ring-2 focus:ring-accent transition-all resize-none ${className}`}
        {...props}
      />
    </div>
  );
};
