import { AxiosResponse } from "axios";
import { useEffect, useState } from "react";

import notFoundBoy from "../assets/404boy.png";
import notFoundGirl from "../assets/404girl.png";
import { useYear } from "../contexts/YearContext";
import {
  debouncedFetchPictures,
  makeDebouncedRequest,
} from "../personalWebsiteApi";
import Picture from "../types/Picture";

interface PictureResponse {
  pictures: Picture[];
}

const usePicturesV2 = () => {
  const [pictures, setPictures] = useState<Picture[]>([]);
  const [loading, setLoading] = useState(false);
  const year = useYear();

  // used to reload pictures when a new picture is added (see NewPictureModal)
  const [trigger, setTrigger] = useState(false);

  useEffect(() => {
    const fetchPictures = () => {
      setLoading(true);

      makeDebouncedRequest(debouncedFetchPictures, {
        url: `/pictures/${year}`,
      })
        .then((response: AxiosResponse<PictureResponse>) => {
          const data = response.data;
          const updatedPictures = (<Picture[]>data.pictures).map((picture) => ({
            ...picture,
            imageUrl: setDefaultImageUrl(picture.imageUrl),
          }));
          setPictures(updatedPictures);
        })
        .catch((error: any) => {
          setPictures([]);
          console.error("Error fetching pictures:", error);
        })
        .finally(() => setLoading(false));
    };

    fetchPictures();
  }, [year, trigger]);

  const setDefaultImageUrl = (imageUrl: string) => {
    return imageUrl || (Math.random() < 0.5 ? notFoundBoy : notFoundGirl);
  };

  return {
    pictures,
    loading,
    setTrigger,
  };
};

export default usePicturesV2;
