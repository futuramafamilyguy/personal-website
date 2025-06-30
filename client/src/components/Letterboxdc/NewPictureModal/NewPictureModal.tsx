import { AxiosResponse } from "axios";
import React, { FormEvent, useEffect, useState } from "react";
import ReactDom from "react-dom";

import {
  createPicture,
  CreatePictureRequest,
  deletePicture,
  getPresignedImageUrl,
  updatePicture,
  UpdatePictureRequest,
  uploadImageToPresignedUrl,
} from "../../../api/pictures";
import { useYear } from "../../../contexts/YearContext";
import {
  debouncedFetchCinemas,
  makeDebouncedRequest,
} from "../../../api/debouncedFetch";
import Cinema from "../../../types/Cinema";
import Picture from "../../../types/Picture";
import styles from "./NewPictureModal.module.css";

interface NewPictureModalProps {
  isOpen: boolean;
  onClose: () => void;
  picture: Picture | null;
  setTrigger: () => void;
}

const NewPictureModal: React.FC<NewPictureModalProps> = ({
  isOpen,
  onClose,
  picture,
  setTrigger,
}) => {
  const [name, setName] = useState("");
  const [yearReleased, setYearReleased] = useState("");
  const [imageFile, setImageFile] = useState<File | null>(null);
  const [imageUrl, setImageUrl] = useState("");
  const [imageObjectKey, setImageObjectKey] = useState("");
  const [monthWatched, setMonthWatched] = useState("");
  const [cinemaId, setCinemaId] = useState("");
  const [zinger, setZinger] = useState("");
  const [alias, setAlias] = useState("");
  const [isFavorite, setIsFavorite] = useState(false);
  const [isKino, setIsKino] = useState(false);
  const [isNewRelease, setIsNewRelease] = useState(true);
  const [altImageFile, setAltImageFile] = useState<File | null>(null);
  const [altImageUrl, setAltImageUrl] = useState("");
  const [altImageObjectKey, setAltImageObjectKey] = useState("");

  const [result, setResult] = useState("");

  const [cinemas, setCinemas] = useState<Cinema[]>([]);
  const year = useYear();

  useEffect(() => {
    setResult("");
    setName(picture ? picture.name : "");
    setYearReleased(picture ? picture.yearReleased.toString() : "");
    setImageFile(null);
    setImageUrl(picture && picture.imageUrl ? picture.imageUrl : "");
    setImageObjectKey(
      picture && picture.imageObjectKey ? picture.imageObjectKey : ""
    );
    setMonthWatched(picture ? picture.monthWatched.toString() : "");
    setCinemaId(picture ? picture.cinema.id : "");
    setZinger(picture && picture.zinger ? picture.zinger : "");
    setAlias(picture && picture.alias ? picture.alias : "");
    setIsFavorite(picture ? picture.isFavorite : false);
    setIsKino(picture ? picture.isKino : false);
    setIsNewRelease(picture ? picture.isNewRelease : true);
    setAltImageFile(null);
    setAltImageUrl(picture && picture.altImageUrl ? picture.altImageUrl : "");
    setAltImageObjectKey(
      picture && picture.altImageObjectKey ? picture.altImageObjectKey : ""
    );
  }, [isOpen]);

  useEffect(() => {
    const fetchCinemas = () => {
      makeDebouncedRequest(debouncedFetchCinemas, {
        url: "/cinemas",
      })
        .then((response: AxiosResponse<Cinema[]>) => {
          setCinemas(response.data);
        })
        .catch((error: any) => {
          console.error("Error fetching cinemas:", error);
        });
    };

    fetchCinemas();
  }, []);

  const orchestrateCreatePicture = async () => {
    try {
      const data: CreatePictureRequest = {
        year: year!,
        name: name,
        monthWatched: parseInt(monthWatched, 10),
        yearReleased: parseInt(yearReleased, 10),
        cinemaId: cinemaId,
        zinger: zinger ? zinger : null,
        alias: alias ? alias : null,
        isFavorite: isFavorite,
        isKino: isKino,
        isNewRelease: isNewRelease,
      };
      const newPicture = await createPicture(data);

      if (imageFile) {
        const extension = imageFile.name.split(".").pop()?.toLowerCase();
        const { presignedUploadUrl } = await getPresignedImageUrl({
          id: newPicture.id,
          extension: extension!,
          isAlt: false,
        });

        await uploadImageToPresignedUrl(presignedUploadUrl, imageFile);
      }
      if (altImageFile) {
        const extension = altImageFile.name.split(".").pop()?.toLowerCase();
        const { presignedUploadUrl } = await getPresignedImageUrl({
          id: newPicture.id,
          extension: extension!,
          isAlt: true,
        });

        await uploadImageToPresignedUrl(presignedUploadUrl, altImageFile);
      }

      setTrigger();
      onClose();
    } catch (error: any) {
      console.error("error creating picture:", error);
      setResult("error creating picture");
    }
  };

  const orchestrateUpdatePicture = async () => {
    try {
      const data: UpdatePictureRequest = {
        id: picture!.id,
        name: name,
        year: year!,
        monthWatched: parseInt(monthWatched, 10),
        yearReleased: parseInt(yearReleased, 10),
        cinemaId: cinemaId,
        zinger: zinger ? zinger : null,
        alias: alias ? alias : null,
        isFavorite: isFavorite,
        isKino: isKino,
        isNewRelease: isNewRelease,
        imageUrl: imageObjectKey ? imageUrl : null, // use object key to determine image presence due to default image
        imageObjectKey: imageObjectKey ? imageObjectKey : null,
        altImageUrl: altImageObjectKey ? altImageUrl : null,
        altImageObjectKey: altImageObjectKey ? altImageObjectKey : null,
      };
      const updatedPicture = await updatePicture(data);

      if (imageFile) {
        const extension = imageFile.name.split(".").pop()?.toLowerCase();
        const { presignedUploadUrl } = await getPresignedImageUrl({
          id: updatedPicture.id,
          extension: extension!,
          isAlt: false,
        });

        await uploadImageToPresignedUrl(presignedUploadUrl, imageFile);
      }
      if (altImageFile) {
        const extension = altImageFile.name.split(".").pop()?.toLowerCase();
        const { presignedUploadUrl } = await getPresignedImageUrl({
          id: updatedPicture.id,
          extension: extension!,
          isAlt: true,
        });

        await uploadImageToPresignedUrl(presignedUploadUrl, altImageFile);
      }

      setTrigger();
      onClose();
    } catch (error: any) {
      console.error("error updating picture:", error);
      setResult("error updating picture");
    }
  };

  const orchestrateDeletePicture = async () => {
    try {
      await deletePicture(picture!.id);

      setTrigger();
      onClose();
    } catch (error: any) {
      console.error("error deleting picture:", error);
      setResult("error deleting picture");
    }
  };

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault();
    if (picture) {
      orchestrateUpdatePicture();
    } else {
      orchestrateCreatePicture();
    }
  };

  const handleDelete = (event: FormEvent) => {
    event.preventDefault();
    orchestrateDeletePicture();
  };

  if (!isOpen) return null;

  return ReactDom.createPortal(
    <>
      <div className={styles.overlay} onClick={onClose}></div>
      <div className={styles.modal}>
        <div className={styles.textContainer}>
          {picture ? (
            <h5 className={styles.title}>update picture</h5>
          ) : (
            <h5 className={styles.title}>add new picture</h5>
          )}
          <form onSubmit={handleSubmit}>
            <div className={styles.formFields}>
              <div className={styles.formGroup}>
                <label>name</label>
                <input
                  type="text"
                  id="name"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  required
                  className={styles.inputField}
                ></input>
              </div>
              <div className={styles.formGroup}>
                <label>year released</label>
                <input
                  type="text"
                  id="year_released"
                  value={yearReleased}
                  onChange={(e) => setYearReleased(e.target.value)}
                  required
                ></input>
              </div>
              <div className={styles.formGroup}>
                <label>image</label>
                <input
                  type="file"
                  onChange={(e) =>
                    setImageFile(e.target.files ? e.target.files[0] : null)
                  }
                />
              </div>
              <div className={styles.formGroup}>
                <label>alt image</label>
                <input
                  type="file"
                  onChange={(e) =>
                    setAltImageFile(e.target.files ? e.target.files[0] : null)
                  }
                />
              </div>
              <div className={styles.formGroup}>
                <label>month watched</label>
                <input
                  type="text"
                  id="month_watched"
                  value={monthWatched}
                  onChange={(e) => setMonthWatched(e.target.value)}
                ></input>
              </div>
              <div className={styles.formGroup}>
                <label>cinema</label>
                <input
                  type="text"
                  id="cinema_id"
                  value={
                    cinemaId !== ""
                      ? cinemas.filter((cinema) => cinema.id === cinemaId)[0]
                          .name
                      : ""
                  }
                  onChange={(e) => setCinemaId(e.target.value)}
                  className={styles.hiddenInput}
                ></input>
                <select
                  id="cinema_id"
                  value={cinemaId}
                  onChange={(e) => setCinemaId(e.target.value)}
                  required
                >
                  <option value="">
                    {cinemaId !== ""
                      ? cinemas.filter((cinema) => cinema.id === cinemaId)[0]
                          .name
                      : "Select a cinema"}
                  </option>
                  {cinemas.map((cinema) => (
                    <option key={cinema.id} value={cinema.id}>
                      {cinema.name}
                    </option>
                  ))}
                </select>
              </div>
              <div className={styles.formGroup}>
                <label>favorite</label>
                <div>
                  <input
                    type="checkbox"
                    id="favorite"
                    checked={isFavorite}
                    onChange={() => setIsFavorite(!isFavorite)}
                  />
                </div>
              </div>
              <div className={styles.formGroup}>
                <label>KINO</label>
                <div>
                  <input
                    type="checkbox"
                    id="kino"
                    checked={isKino}
                    onChange={() => setIsKino(!isKino)}
                  />
                </div>
              </div>
              <div className={styles.formGroup}>
                <label>new release</label>
                <div>
                  <input
                    type="checkbox"
                    id="new_release"
                    checked={isNewRelease}
                    onChange={() => setIsNewRelease(!isNewRelease)}
                  />
                </div>
              </div>
              <div className={styles.formGroup}>
                <label>zinger</label>
                <input
                  type="text"
                  id="zinger"
                  value={zinger}
                  onChange={(e) => setZinger(e.target.value)}
                ></input>
              </div>
              <div className={styles.formGroup}>
                <label>alias</label>
                <input
                  type="text"
                  id="alias"
                  value={alias}
                  onChange={(e) => setAlias(e.target.value)}
                ></input>
              </div>
            </div>
            <div className={styles.buttonContainer}>
              <div>
                <button className={styles.button} type="submit">
                  submit
                </button>
                <span className={styles.result} id="result">
                  {result}
                </span>
              </div>
              {picture ? (
                <div>
                  <button
                    className={styles.deleteButton}
                    onClick={handleDelete}
                  >
                    delete
                  </button>
                </div>
              ) : null}
            </div>
          </form>
        </div>
      </div>
    </>,
    document.getElementById("portal")!
  );
};

export default NewPictureModal;
