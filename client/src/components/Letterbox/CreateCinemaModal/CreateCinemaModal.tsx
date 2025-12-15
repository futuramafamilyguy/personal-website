import React, { FormEvent, useState } from "react";
import ReactDom from "react-dom";

import { createCinema, CreateCinemaRequest } from "../../../api/cinemas";
import styles from "./CreateCinemaModal.module.css";

interface CreateMovieModalProps {
  isOpen: boolean;
  onClose: () => void;
  setTrigger: () => void;
}

const CreateCinemaModal: React.FC<CreateMovieModalProps> = ({
  isOpen,
  onClose,
  setTrigger,
}) => {
  const [name, setName] = useState("");
  const [city, setCity] = useState("");

  const [result, setResult] = useState("");

  const orchestrateCreateCinema = async () => {
    try {
      const data: CreateCinemaRequest = {
        name: name,
        city: city,
      };
      await createCinema(data);

      setTrigger();
      onClose();
    } catch (error: any) {
      console.error("error creating cinema:", error);
      setResult("error creating cinema");
    }
  };

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault();
    orchestrateCreateCinema();
  };

  if (!isOpen) return null;

  return ReactDom.createPortal(
    <>
      <div className={styles.overlay} onClick={onClose}></div>
      <div className={styles.modal}>
        <div className={`${styles.textContainer} bg-dark`}>
          <h5 className={styles.title}>create cinema</h5>
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
                <label>city</label>
                <input
                  type="text"
                  id="city"
                  value={city}
                  onChange={(e) => setCity(e.target.value)}
                  required
                  className={styles.inputField}
                ></input>
              </div>
            </div>
            <div className={styles.buttonContainer}>
              <div>
                <button className={`${styles.button} bg-dark`} type="submit">
                  submit
                </button>
                <span className={styles.result} id="result">
                  {result}
                </span>
              </div>
            </div>
          </form>
        </div>
      </div>
    </>,
    document.getElementById("portal")!
  );
};

export default CreateCinemaModal;
