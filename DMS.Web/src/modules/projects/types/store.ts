export interface ProjectList {
    projects?: Project[];
}

export interface Project {
    id?: number;
    title?: string;
    description?: string;
    startDate?: Date;
    endDate?: Date;
    projectCategory?: string;
    projectCategoryId?: string;
}