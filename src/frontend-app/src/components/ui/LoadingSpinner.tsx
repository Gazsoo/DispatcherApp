export function LoadingSpinner({ size = 'md' }: { size?: 'sm' | 'md' | 'lg' }) {
    const sizeClasses = {
        sm: 'h-6 w-6',
        md: 'h-12 w-12',
        lg: 'h-16 w-16'
    };

    return (
        <div className="flex items-center justify-center min-h-[400px]">
            <div className={`animate-spin rounded-full border-b-2 border-accent ${sizeClasses[size]}`} />
        </div>
    );
}