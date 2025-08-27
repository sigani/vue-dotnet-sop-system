export interface Breadcrumb {
  title: string
  to: string
  disabled?: boolean
}

export interface Category {
  id: number
  name: string
  parentCategoryId?: number | null
}

export interface SOP {
  id: number;
  name: string;
  sopItems: SOPItem[];
  categoryId?: number;
  createdAt: string;
  updatedAt: string;
}

export interface SOPItem {
  id: number;
  sopId: number;
  name: string;
  content: string;
  imagePath?: string;
  stepNumber?: number;
  sortOrder: number;
  imageUrl?: string;
  image?: File | null;
}
