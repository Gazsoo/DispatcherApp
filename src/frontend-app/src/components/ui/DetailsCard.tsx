import type { ReactNode } from 'react';
import { Card } from './Card';

export interface DetailItem {
    label: string;
    value: ReactNode;
}

interface DetailsCardProps {
    items: DetailItem[];
    description?: {
        title: string;
        content: ReactNode;
    };
    className?: string;
}

export const DetailsCard = ({
    items,
    description,
    className = '',
}: DetailsCardProps) => {
    return (
        <Card className={`p-6 space-y-6 ${className}`}>
            {/* Details Grid */}
            {items.length > 0 && (
                <dl className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                    {items.map((item, idx) => (
                        <div key={idx}>
                            <dt className="text-sm text-gray-500">{item.label}</dt>
                            <dd className="text-base font-medium text-gray-800 dark:text-gray-200">
                                {item.value}
                            </dd>
                        </div>
                    ))}
                </dl>
            )}

            {/* Description Section */}
            {description && (
                <div>
                    <h2 className="text-sm text-gray-500 mb-2">{description.title}</h2>
                    <p className="text-base text-gray-800 dark:text-gray-200 whitespace-pre-wrap">
                        {description.content}
                    </p>
                </div>
            )}
        </Card>
    );
};
