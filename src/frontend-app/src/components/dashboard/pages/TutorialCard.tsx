import { Link } from "react-router-dom";
import { Card } from "../../ui";
import type { TutorialResponse } from "../../../services/web-api-client";

interface TutorialCardProps {
    tutorial: TutorialResponse;
}

export function TutorialCard({ tutorial }: TutorialCardProps) {
    // Check if tutorial has an image file
    const imageFile = tutorial.files?.find(file =>
        file.contentType?.startsWith('image/')
    );

    return (
        <Link to={`/dashboard/tutorials/${tutorial.id}`}>
            <Card className="overflow-hidden hover:shadow-lg transition-shadow cursor-pointer h-full">
                {/* Image preview or placeholder */}
                <div className="w-full h-48 bg-gradient-to-br from-blue-100 to-purple-100 dark:from-blue-900 dark:to-purple-900 flex items-center justify-center">
                    {imageFile ? (
                        <img
                            src={`/api/tutorials/${tutorial.id}/files/${imageFile.id}`}
                            alt={tutorial.title}
                            className="w-full h-full object-cover"
                        />
                    ) : (
                        <svg className="w-16 h-16 text-gray-accent" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477 4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
                        </svg>
                    )}
                </div>

                {/* Content */}
                <div className="p-1">
                    <h2 className="text-xl font-semibold mb-2">{tutorial.title}</h2>
                    <p className="text-gray-600 dark:text-gray-400 text-sm line-clamp-2">
                        {tutorial.description || 'No description available'}
                    </p>
                </div>
            </Card>
        </Link>
    );
}